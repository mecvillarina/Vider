namespace Client.App.Infrastructure.Routes
{
    public static class FeedEndpoints
    {
        public const string GetRecentPosts = "api/creatorportal/feeds/recent";
        public const string GetPosts = "api/creatorportal/feeds/posts?creatorId={0}";
        public const string CreatePost = "api/creatorportal/feeds/post";
        public const string DeletePost = "api/creatorportal/feeds/post/delete";
        public const string LikePost = "api/creatorportal/feeds/post/like";
    }
}
