namespace DotNetCoreAngular.Models.Entity
{
    public class UserLike : BaseEntity
    {
        public User SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public User LikedUser { get; set; }
        public int LikedUserId { get; set; }
    }
}
