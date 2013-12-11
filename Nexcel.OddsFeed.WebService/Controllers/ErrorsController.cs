// --------------------------------------------------------------------------
// <copyright file="ErrorsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.WebService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using MvcContrib.ActionResults;
    using Nexcel.OddsFeed.Library;

    /// <summary>
    /// Class ErrorsController.
    /// </summary>    
    public class ErrorsController : Controller
    {        
        // GET: /Error/    
        public XmlResult Index(Exception exception)
        {            
            var error = new Error(4, exception.Message).ToXmlElement();
            return new XmlResult(error);
        }

        public ActionResult General(Exception exception)
        {
            return Content("General failure", "text/plain");
        }
    }
}
