using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Foundation.Models
{
    public class ViewBagFilter : IActionFilter
    {
        private static readonly string Enabled = "Enabled";
        private static readonly string Disabled = string.Empty;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                // SmartAdmin Toggle Features
                
                controller.ViewBag.AppSidebar = Enabled;
                controller.ViewBag.AppHeader = Enabled;
                controller.ViewBag.AppLayoutShortcut = Enabled;
                controller.ViewBag.AppFooter = Enabled;
                controller.ViewBag.ShortcutMenu = Enabled;
                controller.ViewBag.ChatInterface = Enabled;
                controller.ViewBag.LayoutSettings = Enabled;

                // SmartAdmin Default Settings
                controller.ViewBag.App = "Ghani Foundation";
                controller.ViewBag.AppName = "Ghani Foundation";
                controller.ViewBag.AppFlavor = "Ghani Foundation";
                controller.ViewBag.AppFlavorSubscript =  "";
                controller.ViewBag.User =  "Super Admin";
                controller.ViewBag.Email =  "info@ghani.com";
                controller.ViewBag.Twitter =  "";
                controller.ViewBag.Avatar =  "avatar-admin.png";
                controller.ViewBag.Version =  "1.0.0";
                controller.ViewBag.Bs4v =  "4.3";
                controller.ViewBag.Logo = "ghani-foundation.png";
                controller.ViewBag.LogoM = "ghani-foundation.png";
                controller.ViewBag.Copyright = DateTime.Now.Year + " © Ghani Foundation";
                controller.ViewBag.CopyrightInverse = DateTime.Now.Year + " © Ghani Foundation";
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
