using Abp.Dependency;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfarsoftResourcePlan.Extention
{
    public static class AbpSessionExtension
    {
        /// <summary>
        /// 获取机构ID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static Guid GetOrganizationId(this IAbpSession session)
        {
            return Guid.Parse(GetClaimValue(ClaimConsts.ClaimTypes.OrganizationId));
        }
        /// <summary>
        /// 获取机构ID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static IReadOnlyList<Guid> GetAuthedOrganizationId(this IAbpSession session)
        {
            return GetClaimValue(ClaimConsts.ClaimTypes.AuthedOrganizationId)?.Split(",")?.Select(e => Guid.Parse(e))?.ToList();
        }
        /// <summary>
        /// 获取机构编码
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetOrganizationCode(this IAbpSession session)
        {
            return GetClaimValue(ClaimConsts.ClaimTypes.OrganizationCode);
        }
        /// <summary>
        /// 获取Erp用户ID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static Guid GetUserInfoId(this IAbpSession session)
        {
            return Guid.Parse(GetClaimValue(ClaimConsts.ClaimTypes.UserInfoId));
        }
        /// <summary>
        /// 获取省级ID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static int? GetProvinceId(this IAbpSession session)
        {
            var val = GetClaimValue(ClaimConsts.ClaimTypes.ProvinceId);
            if (string.IsNullOrEmpty(val))
                return null;
            return int.Parse(val);
        }
        /// <summary>
        /// 获取市级ID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static int? GetCityId(this IAbpSession session)
        {
            var val = GetClaimValue(ClaimConsts.ClaimTypes.CityId);
            if (string.IsNullOrEmpty(val))
                return null;
            return int.Parse(val);
        }
        /// <summary>
        /// 获取区级ID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static int? GetRegionId(this IAbpSession session)
        {
            var val = GetClaimValue(ClaimConsts.ClaimTypes.RegionId);
            if (string.IsNullOrEmpty(val))
                return null;
            return int.Parse(val);
        }
        /// <summary>
        /// 获取区域关系关联字段
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetAreaIdRealation(this IAbpSession session)
        {
            return GetClaimValue(ClaimConsts.ClaimTypes.AreaIdRealation);
        }
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetUserName(this IAbpSession session)
        {
            return GetClaimValue(ClaimConsts.ClaimTypes.UserName);
        }
        /// <summary>
        /// 获取区域名称关系关联字段
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetAreaNameRealation(this IAbpSession session)
        {
            return GetClaimValue(ClaimConsts.ClaimTypes.AreaNameRealation);
        }
        private static string GetClaimValue(string claimType)
        {
            var PrincipalAccessor = IocManager.Instance.Resolve<IPrincipalAccessor>();
            var claimsPrincipal = PrincipalAccessor.Principal;
            var claim = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
            if (string.IsNullOrEmpty(claim?.Value))
                return null;

            return claim.Value;
        }
    }
}
