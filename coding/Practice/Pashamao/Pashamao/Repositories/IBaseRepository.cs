using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Repositories
{
    public abstract class IBaseRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        protected abstract string ConnStr{ get; }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal abstract void Create( TEntity entity );
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal abstract void Update( TEntity entity );
        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal abstract void Delete( TEntity entity );
        /// <summary>
        /// 取得一筆資料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal abstract TEntity Get( int primaryId );
        /// <summary>
        /// 取得多筆資料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal abstract IEnumerable<TEntity> GetAll();




    }
}