using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Transactions;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public static class UpdateUtil
    {
        public static void UpdateTableResolvers(TkDbContext context, Action<Transaction> applyData,
            params TableResolver[] resolvers)
        {
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            UpdateTableResolvers(context, applyData, resolvers as IEnumerable<TableResolver>);
        }

        public static void UpdateTableResolvers(TkDbContext context, Action<Transaction> applyData,
            bool sort, params TableResolver[] resolvers)
        {
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            UpdateTableResolvers(context, applyData, sort, resolvers as IEnumerable<TableResolver>);
        }

        public static void UpdateTableResolvers(TkDbContext context, Action<Transaction> applyData,
            TableResolver resolver, IEnumerable<TableResolver> resolvers)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            UpdateTableResolvers(context, applyData, EnumUtil.Convert(resolver, resolvers));
        }

        public static void UpdateTableResolvers(TkDbContext context, Action<Transaction> applyData,
            IEnumerable<TableResolver> resolvers)
        {
            UpdateTableResolvers(context, applyData, true, resolvers);
        }

        public static void UpdateTableResolvers(TkDbContext context, Action<Transaction> applyData,
            bool sort, TableResolver resolver, IEnumerable<TableResolver> resolvers)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            UpdateTableResolvers(context, applyData, sort, EnumUtil.Convert(resolver, resolvers));
        }

        public static void UpdateTableResolvers(TkDbContext context, Action<Transaction> applyData,
            bool sort, IEnumerable<TableResolver> resolvers)
        {
            TkDebug.AssertArgumentNull(context, "context", null);
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            TkDebug.Assert(context.DbConnection is DbConnection, string.Format(ObjectUtil.SysCulture,
                "名称为{0}的DbContext中的DbConnection不是从System.Data.Common.DbConnection继承的，无法支持事务",
                context.ContextConfig.Name), null);
            DbConnectionStatus item = new DbConnectionStatus(context.DbConnection);

            CommittableTransaction transaction = CreateTransaction();
            using (transaction)
            {
                item.AttachTransaction(transaction);
                try
                {
                    ApplyUpdates(transaction, applyData, sort, resolvers);
                }
                catch (DBConcurrencyException ex)
                {
                    transaction.Rollback();

                    if (ex.Message.IndexOf("UpdateCommand 影响 0 个记录", StringComparison.CurrentCulture) > 0)
                        throw new ConcurrencyException(ex);
                    else
                        throw;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    item.DetachTransaction();
                }
            }
        }

        public static void UpdateTableResolvers(TkDbContext context, Action<Transaction> applyData)
        {
            TkDebug.AssertArgumentNull(context, "context", null);
            TkDebug.AssertArgumentNull(applyData, "applyData", null);

            TkDebug.Assert(context.DbConnection is DbConnection, string.Format(ObjectUtil.SysCulture,
                "名称为{0}的DbContext中的DbConnection不是从System.Data.Common.DbConnection继承的，无法支持事务",
                context.ContextConfig.Name), null);
            DbConnectionStatus item = new DbConnectionStatus(context.DbConnection);

            CommittableTransaction transaction = CreateTransaction();
            using (transaction)
            {
                item.AttachTransaction(transaction);
                try
                {
                    applyData(transaction);
                }
                catch (DBConcurrencyException ex)
                {
                    transaction.Rollback();

                    if (ex.Message.IndexOf("UpdateCommand 影响 0 个记录", StringComparison.CurrentCulture) > 0)
                        throw new ConcurrencyException(ex);
                    else
                        throw;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    item.DetachTransaction();
                }
            }
        }

        private static void ApplyUpdates(CommittableTransaction transaction,
            Action<Transaction> applyData, bool sort, IEnumerable<TableResolver> resolvers)
        {
            if (sort)
                resolvers = resolvers.OrderBy((resolver) => resolver.TableName);
            foreach (TableResolver resolver in resolvers)
            {
                if (resolver != null)
                    resolver.InternalUpdateDatabase();
            }
            if (applyData != null)
                applyData(transaction);
            transaction.Commit();
        }

        private static CommittableTransaction CreateTransaction()
        {
            TransactionOptions options = new TransactionOptions()
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = BaseAppSetting.Current != null ?
                    new TimeSpan(0, 0, BaseAppSetting.Current.CommandTimeout) :
                    TransactionManager.DefaultTimeout
            };

            CommittableTransaction transaction = new CommittableTransaction(options);
            return transaction;
        }

        public static void UpdateTableResolvers(IEnumerable<TkDbContext> contexts,
            Action<Transaction> applyData, params TableResolver[] resolvers)
        {
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            UpdateTableResolvers(contexts, applyData, resolvers as IEnumerable<TableResolver>);
        }

        public static void UpdateTableResolvers(IEnumerable<TkDbContext> contexts,
            Action<Transaction> applyData, bool sort, params TableResolver[] resolvers)
        {
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            UpdateTableResolvers(contexts, applyData, sort, resolvers as IEnumerable<TableResolver>);
        }

        public static void UpdateTableResolvers(IEnumerable<TkDbContext> contexts,
            Action<Transaction> applyData, TableResolver resolver,
            IEnumerable<TableResolver> resolvers)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            UpdateTableResolvers(contexts, applyData, EnumUtil.Convert(resolver, resolvers));
        }

        public static void UpdateTableResolvers(IEnumerable<TkDbContext> contexts,
            Action<Transaction> applyData, bool sort, TableResolver resolver,
            IEnumerable<TableResolver> resolvers)
        {
            TkDebug.AssertArgumentNull(resolver, "resolver", null);
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            UpdateTableResolvers(contexts, applyData, sort, EnumUtil.Convert(resolver, resolvers));
        }

        public static void UpdateTableResolvers(IEnumerable<TkDbContext> contexts,
            Action<Transaction> applyData, IEnumerable<TableResolver> resolvers)
        {
            UpdateTableResolvers(contexts, applyData, true, resolvers);
        }

        public static void UpdateTableResolvers(IEnumerable<TkDbContext> contexts,
            Action<Transaction> applyData, bool sort, IEnumerable<TableResolver> resolvers)
        {
            TkDebug.AssertEnumerableArgumentNull(contexts, "contexts", null);
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            List<DbConnectionStatus> contextItems = new List<DbConnectionStatus>(contexts.Count());

            foreach (TkDbContext context in contexts)
            {
                TkDebug.Assert(context.DbConnection is DbConnection, string.Format(ObjectUtil.SysCulture,
                    "名称为{0}的DbContext中的DbConnection不是从System.Data.Common.DbConnection继承的，无法支持事务",
                    context.ContextConfig.Name), null);
                contextItems.Add(new DbConnectionStatus(context.DbConnection));
            }

            CommittableTransaction transaction = CreateTransaction();
            using (transaction)
            {
                foreach (DbConnectionStatus item in contextItems)
                    item.AttachTransaction(transaction);
                try
                {
                    ApplyUpdates(transaction, applyData, sort, resolvers);
                }
                catch (DBConcurrencyException ex)
                {
                    transaction.Rollback();

                    if (ex.Message.IndexOf("UpdateCommand 影响 0 个记录", StringComparison.CurrentCulture) > 0)
                        throw new ConcurrencyException(ex);
                    else
                        throw;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    foreach (DbConnectionStatus item in contextItems)
                        item.DetachTransaction();
                }
            }
        }

        public static void UpdateTableResolvers(Action<Transaction> applyData,
            params TableResolver[] resolvers)
        {
            UpdateTableResolvers(applyData, resolvers as IEnumerable<TableResolver>);
        }

        public static void UpdateTableResolvers(Action<Transaction> applyData,
            bool sort, params TableResolver[] resolvers)
        {
            UpdateTableResolvers(applyData, sort, resolvers as IEnumerable<TableResolver>);
        }

        public static void UpdateTableResolvers(Action<Transaction> applyData,
            IEnumerable<TableResolver> resolvers)
        {
            UpdateTableResolvers(applyData, true, resolvers);
        }

        public static void UpdateTableResolvers(Action<Transaction> applyData,
            bool sort, IEnumerable<TableResolver> resolvers)
        {
            TkDebug.AssertArgumentNull(resolvers, "resolvers", null);

            IEnumerable<TkDbContext> contexts = (from resolver in resolvers
                                                 select resolver.Context).Distinct();
            List<TkDbContext> contextArray = contexts.ToList();
            int count = contextArray.Count;
            if (count == 0)
                return;
            if (count == 1)
                UpdateTableResolvers(contextArray[0], applyData, sort, resolvers);
            else
                UpdateTableResolvers(contextArray, applyData, sort, resolvers);
        }
    }
}
