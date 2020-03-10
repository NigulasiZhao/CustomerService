using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AfarsoftResourcePlan.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AfarsoftResourcePlan.Web.Mvc.Controllers
{
    public class TestController : AfarsoftResourcePlanControllerBase
    {
        /// <summary>
        /// 查询类
        /// </summary>
        /// <returns></returns>
        public IActionResult Query()
        {
            return View();
        }
        /// <summary>
        /// 跟踪类
        /// </summary>
        /// <returns></returns>
        public IActionResult Track()
        {
            return View();
        }
        /// <summary>
        /// 报表类
        /// </summary>
        /// <returns></returns>
        public IActionResult Report()
        {
            return View();
        }
        /// <summary>
        /// 带选项卡的编辑页
        /// </summary>
        /// <returns></returns>
        public IActionResult EditWithTab()
        {
            return View();
        }
        /// <summary>
        /// 基础资料管理（弹窗带选项卡）
        /// </summary>
        /// <returns></returns>
        public IActionResult InfoManagerWithDialogAndTab()
        {
            return View();
        }
        /// <summary>
        /// 基础资料管理（弹窗不带选项卡）
        /// </summary>
        /// <returns></returns>
        public IActionResult InfoManagerWithDialogWithoutTab()
        {
            return View();
        }
        /// <summary>
        /// 商品列表页（左树右表格）
        /// </summary>
        /// <returns></returns>
        public IActionResult GoodsList()
        {
            return View();
        }
        /// <summary>
        /// 制单页
        /// </summary>
        /// <returns></returns>
        public IActionResult Order()
        {
            return View();
        }
        /// <summary>
        /// 设置页
        /// </summary>
        /// <returns></returns>
        public IActionResult Setting()
        {
            return View();
        }
        /// <summary>
        /// 权限页
        /// </summary>
        /// <returns></returns>
        public IActionResult Permission()
        {
            return View();
        }
        /// <summary>
        /// 调价单
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangePrice()
        {
            return View();
        }
        /// <summary>
        /// 调价单
        /// </summary>
        /// <returns></returns>
        public IActionResult TreeGrid()
        {
            return View();
        }
    }
}