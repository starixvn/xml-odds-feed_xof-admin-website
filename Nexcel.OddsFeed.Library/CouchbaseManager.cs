// --------------------------------------------------------------------------
// <copyright file="CouchbaseManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.04.03">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Couchbase;

    /// <summary>
    /// Class Couch base Manager. This class cannot be inherited.
    /// </summary>
    public sealed class CouchbaseManager
    {
        private static volatile CouchbaseClient _instance;
        private static object syncRoot = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="CouchbaseManager"/> class from being created.
        /// </summary>
        private CouchbaseManager()
        {            
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static CouchbaseClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new CouchbaseClient();
                        }
                    }
                }

                return _instance;
            }
        }
    }    
}
