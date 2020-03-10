using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using AfarsoftResourcePlan.Authorization.Roles;
using Abp.Domain.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace AfarsoftResourcePlan.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        //private readonly IRepository<Domain.Erp.BasicInfo.UserInfo, Guid> _UserInfoRepository;
        //private readonly IRepository<Domain.Erp.Organization, Guid> _OrganizationRepository;
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor
            /*,IRepository<Domain.Erp.BasicInfo.UserInfo, Guid> repository, IRepository<Domain.Erp.Organization, Guid> organizationRepository*/)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor)
        {
            //_UserInfoRepository = repository;
            //_OrganizationRepository = organizationRepository;
        }
        //public override async Task<ClaimsPrincipal> CreateAsync(User user)
        //{
        //    var principal = await base.CreateAsync(user);

        //    var identity = principal.Identities.First();
        //    var UserInfo = await GetUserInfoByAbpUserId(user.Id);
        //    //这个常量自己去定义
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.OrganizationId, UserInfo.OrganizationId.ToString()));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.OrganizationCode, UserInfo.Organization.OrganizationCode == null ? "" : UserInfo.Organization.OrganizationCode));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.UserInfoId, UserInfo.Id.ToString()));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.UserName, UserInfo.UserName == null ? "" : UserInfo.UserName));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.ProvinceId, UserInfo.ProvinceId.ToString()));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.CityId, UserInfo.CityId.ToString()));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.RegionId, UserInfo.RegionId.ToString()));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.AreaIdRealation, UserInfo.AreaIdRealation == null ? "" : UserInfo.AreaIdRealation));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.AreaNameRealation, UserInfo.AreaNameRealation == null ? "" : UserInfo.AreaNameRealation));
        //    identity.AddClaim(new Claim(ClaimConsts.ClaimTypes.AuthedOrganizationId, UserInfo.OrganizationPermission == null ? "" : UserInfo.OrganizationPermission));


        //    return principal;
        //}
        //public async Task<Domain.Erp.BasicInfo.UserInfo> GetUserInfoByAbpUserId(long AbpUserId)
        //{
        //    List<Domain.Erp.BasicInfo.UserInfo> list = await _UserInfoRepository.GetAllListAsync(x => x.UserId == AbpUserId);
        //    if (list.Count == 0)
        //    {
        //        Domain.Erp.BasicInfo.UserInfo UserInfoModel = new Domain.Erp.BasicInfo.UserInfo();
        //        UserInfoModel.Organization = new Domain.Erp.Organization();
        //        return UserInfoModel;
        //    }
        //    list[0].Organization = await _OrganizationRepository.GetAsync(list[0].OrganizationId);
        //    return list[0];
        //}
    }
}
