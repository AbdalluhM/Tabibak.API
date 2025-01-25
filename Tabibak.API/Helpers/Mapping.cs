namespace Tabibak.Api.Helpers
{
    public class Mapping
    {
        //public class GetCategoryWishlistStatusResolver : IMemberValueResolver<object, object, int, bool>
        //{
        //    private readonly IHttpContextAccessor _httpContextAccessor;
        //    private readonly ApplicationDbcontext _dbcontext;

        //    public GetCategoryWishlistStatusResolver(IHttpContextAccessor httpContextAccessor, ApplicationDbcontext dbcontext)
        //    {
        //        _httpContextAccessor = httpContextAccessor;
        //        _dbcontext = dbcontext;
        //    }

        //    public bool Resolve(object source, object destination, int sourceMember, bool destMember, ResolutionContext context)
        //    {
        //        var userId = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "uid").Value.ToString() ?? string.Empty;
        //        return _dbcontext.CategoryWishlists.Any(w => w.CategoryId == sourceMember && w.UserId == userId);
        //    }
        //}

        //public class GetUrlResolver : IMemberValueResolver<object, object, int, string>
        //{
        //    private readonly IHttpContextAccessor _httpContextAccessor;
        //    private readonly ApplicationDbcontext _dbcontext;

        //    public GetUrlResolver(IHttpContextAccessor httpContextAccessor, ApplicationDbcontext dbcontext)
        //    {
        //        _httpContextAccessor = httpContextAccessor;
        //        _dbcontext = dbcontext;
        //    }

        //    public string Resolve(object source, object destination, int sourceMember, string destMember, ResolutionContext context)
        //    {
        //        return Path.Combine(
        //    }
        //}
    }
}
