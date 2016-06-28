// --------------------------------------------------------------------------
// <copyright file="EntityBase.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.6">Created</change>
//   <change who="Thai.Tran" date="2013.11.29">apply FXCop</change>
// </history>

namespace Fanex.OddsFeed.Framework.Domain
{
    using System;

    /// <summary>
    /// Base entity.
    /// </summary>
    /// <typeparam name="TId">The type of the id.</typeparam>
    /// <typeparam name="TName">The type of the name.</typeparam>
    public abstract class EntityBase<TId, TName> : IEntity<TId, TName>
    {
        private TId _id;
        private TName _name;

        public virtual TId Id
        {
            get
            {
                return this._id;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "The id property cannot be set to null.");
                }               

                this._id = value;
            }
        }

        public virtual TName Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }  
    }
}
