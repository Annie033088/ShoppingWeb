using System;
using System.Collections.Generic;

namespace Pashamao.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        protected override string ConnStr
        {
            get { return System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString; }
        }

        internal override void Create( TEntity entity )
        {
            throw new NotImplementedException ();
        }
        internal override void Update( TEntity entity )
        {
            throw new NotImplementedException ();
        }

        internal override void Delete( TEntity entity )
        {
            throw new NotImplementedException ();
        }

        internal override TEntity Get( int primaryId )
        {
            throw new NotImplementedException ();
        }

        internal override IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException ();
        }


    }
}